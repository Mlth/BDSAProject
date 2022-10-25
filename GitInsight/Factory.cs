namespace GitInsight;
class Factory{
    
    public static Command getCommand(string mode){
        switch (mode){
            case "frequency":
                return new FrequencyCommand();
            case "author":
                return new AuthorCommand();
            default:
                throw new NotImplementedException();
        }
    }

}