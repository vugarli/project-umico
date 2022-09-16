using System.Reflection.Metadata.Ecma335;

namespace ProjectUmico.Application.Dtos;

public interface ICachable
{
    public DateTime? LastModified { get; set; }
}