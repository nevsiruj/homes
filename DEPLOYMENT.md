# Fix for Broken Apartment Links

## Problem
Links to apartments on the public website (homes-web) were not working because the `SlugInmueble` field in the database was not being populated. The API endpoint `/Inmueble/by-slug/{slug}` requires this field to find properties.

## Solution Overview
This fix includes:
1. **Backend (API)**: Auto-generate slugs for all properties (new and existing)
2. **Frontend**: Add fallback slug generation on the client-side
3. **Database Migration**: Populate slugs for all existing properties

## Deployment Instructions

### Step 1: Deploy API Changes

1. Deploy the updated API code to production
2. The new code includes:
   - `SlugGenerator.cs` utility for generating URL-friendly slugs
   - Updated `InmuebleService` to auto-generate slugs on create/update
   - New migration `20251221021500_PopulateInmuebleSlugs.cs`

### Step 2: Run Database Migration

**IMPORTANT**: This migration will update all properties in the database that have missing slugs.

```bash
# Navigate to the API directory
cd api

# Run the migration
dotnet ef database update

# Or if using specific connection string:
dotnet ef database update --connection "YourConnectionString"
```

**What the migration does:**
- Generates slugs for all properties where `SlugInmueble` is NULL or empty
- Combines `Titulo` (title) and `CodigoPropiedad` (property code) to create unique slugs
- Removes accents, converts to lowercase, and replaces spaces with hyphens
- Example: "Apartamento en Venta Zona 14" + "ASV8508" → "apartamento-en-venta-zona-14-asv8508"

### Step 3: Deploy Frontend Changes

1. Deploy the updated homes-web code to production
2. The frontend now includes fallback slug generation in:
   - `InmuebleCard.vue`
   - `SugerenciaCard.vue`
   - `pages/home/seccion1.vue`

### Step 4: Verify

1. Test the example URL: `https://homesguatemala.com/inmueble/apartamento-en-venta-zona-14-asv8508`
2. Check that property links work throughout the site
3. Verify that suggested properties have working links

## How It Works

### Backend Slug Generation
```csharp
// Automatically generates: "apartamento-en-venta-zona-14-asv8508"
string slug = SlugGenerator.GenerateSlug("Apartamento en Venta Zona 14", "ASV8508");
```

### Frontend Fallback
If the API doesn't provide a slug, the frontend generates one client-side:
```javascript
const generateClientSlug = (item) => {
  const text = `${item.titulo} ${item.codigoPropiedad}`.trim();
  return text
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')  // Remove accents
    .replace(/[^a-z0-9]+/g, '-')      // Replace with hyphens
    .replace(/^-+|-+$/g, '');          // Clean up
};
```

## Rollback Plan

If issues occur after deployment:

1. **Rollback API**: Deploy previous API version
2. **Rollback Database**: The migration includes a `Down()` method that sets all slugs to NULL
   ```bash
   dotnet ef database update <PreviousMigrationName>
   ```
3. **Rollback Frontend**: Deploy previous frontend version

## Technical Details

### Slug Format
- Lowercase letters and numbers only
- Hyphens separate words
- Spanish accents removed (á→a, é→e, etc.)
- Maximum length: 200 characters
- Format: `{title}-{property-code}`

### Database Changes
- Table: `Inmuebles`
- Column: `SlugInmueble` (nvarchar(max), nullable)
- Existing column, just populating values

### API Changes
- Files modified:
  - `api/Application/Services/Common/SlugGenerator.cs` (new)
  - `api/Application/Modules/InmuebleModule/Service/InmuebleService.cs`
  - `api/Infrastructure/Migrations/20251221021500_PopulateInmuebleSlugs.cs` (new)

### Frontend Changes
- Files modified:
  - `homes-web/components/InmuebleCard.vue`
  - `homes-web/components/SugerenciaCard.vue`
  - `homes-web/pages/home/seccion1.vue`

## Future Considerations

1. **Slug Uniqueness**: Current implementation doesn't enforce unique slugs. Consider adding a unique index if needed.
2. **Slug Updates**: If a property's title or code changes, the slug will be regenerated on update.
3. **SEO**: Existing URLs may change if the slug generation produces different results. Consider adding redirects if needed.

## Testing Checklist

- [ ] Migration runs successfully without errors
- [ ] All properties have populated `SlugInmueble` values
- [ ] Example URL works: `/inmueble/apartamento-en-venta-zona-14-asv8508`
- [ ] Property cards show correct links on homepage
- [ ] Property cards show correct links on properties page
- [ ] Suggested properties have working links
- [ ] New properties get slugs automatically
- [ ] Updated properties get slugs if missing
- [ ] CRM property copying URL functionality works
