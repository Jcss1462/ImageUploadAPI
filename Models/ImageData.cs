using System.ComponentModel.DataAnnotations;

namespace ImageUploadAPI.Models
{
    public class ImageData
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Dorsal { get; set; }
        public required byte[] Data { get; set; }
    }
}
