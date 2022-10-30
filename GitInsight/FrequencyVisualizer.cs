namespace GitInsight;

public class FrequencyVisualizer : IVisualizer{
    List<FrequencyDTO> dtos;
    public FrequencyVisualizer(List<FrequencyDTO> dtos){
        this.dtos = dtos;
    }     public void visualize(){
        foreach (FrequencyDTO dto in dtos){
            Console.WriteLine(dto.frequency + " " + dto.dateTime);
        }
    }
}