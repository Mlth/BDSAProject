namespace GitInsight;
class Factory{
    
    public static AbstractCommand getCommand(string mode){
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