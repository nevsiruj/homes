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
            // SQL function to generate slugs (simplified version for SQL Server)
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
                                                                        REPLACE(TRIM(CONCAT(Titulo, '-', CodigoPropiedad)), ' ', '-'),
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
                WHERE SlugInmueble IS NULL OR SlugInmueble = ''
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

            // Remove multiple consecutive hyphens
            migrationBuilder.Sql(@"
                DECLARE @Continue BIT = 1;
                WHILE @Continue = 1
                BEGIN
                    UPDATE Inmuebles
                    SET SlugInmueble = REPLACE(SlugInmueble, '--', '-')
                    WHERE CHARINDEX('--', SlugInmueble) > 0;
                    
                    IF @@ROWCOUNT = 0
                        SET @Continue = 0;
                END
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
