namespace GitInsight;
using GitInsight.Entities.DTOS;

public class FrequencyVisualizer : IVisualizer{
    IEnumerable<FrequencyDTO> dtos;
    public FrequencyVisualizer(IEnumerable<FrequencyDTO> dtos){
        this.dtos = dtos;
    }     public void visualize(){
        foreach (FrequencyDTO dto in dtos){
            Console.WriteLine(dto.frequency + " " + dto.date.ToShortDateString());
        }
    }
}