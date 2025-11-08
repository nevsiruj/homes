using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProyectoDTO
{
    public int Id { get; set; }
    public string? CodigoProyecto { get; set; }
    //public Proyecto? Proyecto { get; set; }
    public string Titulo { get; set; }
    public string? SlugProyecto { get; set; } = null;
    public decimal Precio { get; set; }
    //public int Habitaciones { get; set; }
    //public int Banos { get; set; }
    //public int Parqueos { get; set; }
    public decimal MetrosCuadrados { get; set; }
    public string? ImagenPrincipal { get; set; }
    public List<ImagenProyectoDTO>? ImagenesReferenciaProyecto { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string? Contenido { get; set; }
    public string? Tipos { get; set; }
    //public string? Operaciones { get; set; }
    public string? Ubicaciones { get; set; }
    public List<AmenidadProyectoDTO>? Amenidades { get; set; }
    //public bool Luxury { get; set; }
    public string? Video { get; set; }
}

public class ImagenProyectoDTO
{
    public string Url { get; set; }
    //public string? Descripcion { get; set; }
}

public class AmenidadProyectoDTO
{
    public int AmenidadProyectoId { get; set; }
    public string Nombre { get; set; }

}
