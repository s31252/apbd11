using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace apbd11.Models;
[PrimaryKey(nameof(IdMedicament),nameof(IdPrescription))]
[Table("Prescription_Medicament")]
public class PrescriptionMedicament
{
    [ForeignKey(nameof(Medicament))]
    public int IdMedicament { get; set; }
    [ForeignKey(nameof(Prescription))]
    public int IdPrescription { get; set; }
    public int? Dose { get; set; }
    [MaxLength(100)]
    public string Details { get; set; }
}