using System;
using System.Collections.Generic;

namespace aspgrupo2.Models;

public partial class SportsArticle
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public string? Categoria { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? CreadoPor { get; set; }

    public virtual User? CreadoPorNavigation { get; set; }
}
