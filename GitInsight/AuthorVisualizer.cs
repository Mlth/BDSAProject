namespace GitInsight;
using GitInsight.Entities.DTOS;

public class AuthorVisualizer : IVisualizer{
    IEnumerable<AuthorDTO> dtos;
    public AuthorVisualizer(IEnumerable<AuthorDTO> dtos){
        this.dtos = dtos;
    }
     public void visualize(){
        foreach(AuthorDTO dto in dtos){
            Console.WriteLine(dto.author);
            foreach(FrequencyDTO freq in dto.frequencies){
                Console.WriteLine("----" + freq.frequency + " " + freq.date.ToShortDateString());
            }
        }
    }
}