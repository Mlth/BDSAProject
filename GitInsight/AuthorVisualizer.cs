namespace GitInsight;

public class AuthorVisualizer : IVisualizer
{
    List<AuthorDTO> dtos;
    public AuthorVisualizer(List<AuthorDTO> dtos)
    {
        this.dtos = dtos;
    }
    public void visualize()
    {
        foreach (AuthorDTO dto in dtos)
        {
            Console.WriteLine(dto.Author);
            foreach (FrequencyDTO freq in dto.frequencies)
            {
                Console.WriteLine("----" + freq.frequency + " " + freq.dateTime);
            }
        }
    }
}