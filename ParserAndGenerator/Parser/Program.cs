
var pathToController =
    "/home/iskander/Desktop/techSpringCrud/src/main/java/com/example/techspringcrud/controllers/TaskController.java";

var pathToModel =
    "/home/iskander/Desktop/techSpringCrud/src/main/java/com/example/techspringcrud/entity/TaskEntity.java";
var streamReaderToController = new StreamReader(pathToController);
var streamReaderToModel = new StreamReader(pathToModel);

/*
var controller = ControllerParser.GetControllerData(streamReader, "Task");
*/

foreach (var methodDto in Parser.Methods.Parser.GetMethods())
{
    Console.WriteLine(methodDto.MethodName);
}