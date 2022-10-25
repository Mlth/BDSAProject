﻿// See https://aka.ms/new-console-template for more information
namespace GitInsight;
using LibGit2Sharp;

public class Program{
    public static void main(string[] args){
        var repository = new Repository(args[0]);
        var command = Factory.getCommand(args[1]);
        command.execute(repository);
    }
}
