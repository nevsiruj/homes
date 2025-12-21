using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PopulateInmuebleSlugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SQL function to generate slugs with proper NULL handling
            migrationBuilder.Sql(@"
                UPDATE Inmuebles
                SET SlugInmueble = 
                    LOWER(
                        REPLACE(
                            REPLACE(
                                REPLACE(
                                    REPLACE(
                                        REPLACE(
                                            REPLACE(
                                                REPLACE(
                                                    REPLACE(
                                                        REPLACE(
                                                            REPLACE(
                                                                REPLACE(
                                                                    REPLACE(
                                                                        REPLACE(
                                                                            TRIM(
                                                                                CASE 
                                                                                    WHEN Titulo IS NOT NULL AND CodigoPropiedad IS NOT NULL 
                                                                                        THEN CONCAT(Titulo, ' ', CodigoPropiedad)
                                                                                    WHEN Titulo IS NOT NULL 
                                                                                        THEN Titulo
                                                                                    WHEN CodigoPropiedad IS NOT NULL 
                                                                                        THEN CodigoPropiedad
                                                                                    ELSE ''
                                                                                END
                                                                            ), ' ', '-'),
                                                                        'á', 'a'),
                                                                    'é', 'e'),
                                                                'í', 'i'),
                                                            'ó', 'o'),
                                                        'ú', 'u'),
                                                    'ñ', 'n'),
                                                'ü', 'u'),
                                            'Á', 'a'),
                                        'É', 'e'),
                                    'Í', 'i'),
                                'Ó', 'o'),
                            'Ú', 'u')
                        )
                    )
                WHERE (SlugInmueble IS NULL OR SlugInmueble = '') 
                  AND (Titulo IS NOT NULL OR CodigoPropiedad IS NOT NULL)
            ");

            // Clean up the slugs to remove special characters and multiple hyphens
            migrationBuilder.Sql(@"
                UPDATE Inmuebles
                SET SlugInmueble = 
                    REPLACE(
                        REPLACE(
                            REPLACE(
                                REPLACE(
                                    REPLACE(
                                        REPLACE(
                                            REPLACE(
                                                REPLACE(
                                                    REPLACE(
                                                        REPLACE(
                                                            REPLACE(
                                                                REPLACE(
                                                                    REPLACE(SlugInmueble, '/', '-'),
                                                                '\\', '-'),
                                                            '(', '-'),
                                                        ')', '-'),
                                                    '[', '-'),
                                                ']', '-'),
                                            '{', '-'),
                                        '}', '-'),
                                    ',', '-'),
                                '.', '-'),
                            '!', '-'),
                        '?', '-'),
                    ':', '-')
                WHERE SlugInmueble IS NOT NULL
            ");

            // Remove multiple consecutive hyphens using PATINDEX
            migrationBuilder.Sql(@"
                UPDATE Inmuebles
                SET SlugInmueble = 
                    REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
                        SlugInmueble, 
                        '-----', '-'), 
                        '----', '-'), 
                        '---', '-'), 
                        '--', '-'),
                        '--', '-')
                WHERE SlugInmueble LIKE '%-%-%'
            ");

            // Trim leading and trailing hyphens
            migrationBuilder.Sql(@"
                UPDATE Inmuebles
                SET SlugInmueble = 
                    CASE 
                        WHEN RIGHT(SlugInmueble, 1) = '-' THEN LEFT(SlugInmueble, LEN(SlugInmueble) - 1)
                        ELSE SlugInmueble
                    END
                WHERE SlugInmueble LIKE '%-'
            ");

            migrationBuilder.Sql(@"
                UPDATE Inmuebles
                SET SlugInmueble = 
                    CASE 
                        WHEN LEFT(SlugInmueble, 1) = '-' THEN SUBSTRING(SlugInmueble, 2, LEN(SlugInmueble) - 1)
                        ELSE SlugInmueble
                    END
                WHERE SlugInmueble LIKE '-%'
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Set all slugs back to null
            migrationBuilder.Sql(@"
                UPDATE Inmuebles
                SET SlugInmueble = NULL
            ");
        }
    }
}
