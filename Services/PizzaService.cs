using ContosoPizza.Models;
using Microsoft.Win32.SafeHandles;
using System;
using System.Text;
using System.Text.Json;

namespace WebApplication1.Services;

public static class PizzaService
{
    static private string fileName = "data.json";
    static List<Pizza> pizzas;
    static int nextId;
    static void findNextId()
    {
        if (!File.Exists(fileName))
        {
            File.Create(fileName).Close();
            nextId = 1;
        }
        else
        {
            nextId = File.ReadAllLines(fileName).Length + 1;
        }
    }
    static PizzaService()
    {
        //Initialize pizzas list based off the data in data.json
        findNextId();
        pizzas = new List<Pizza>();
        string[] lines;
        lines = File.ReadAllLines(fileName);
        Pizza? pizza = new Pizza();
        foreach (string line in lines)
        {
            pizza = JsonSerializer.Deserialize<Pizza>(line);
            if (pizza != null) pizzas.Add(pizza);
        }
    }

    ///<summary>
    ///Add a pizza to the list and to the json file.
    ///</summary>
    public static void Add(Pizza pizza)
    {

        pizza.Id = nextId++;
        pizzas.Add(pizza);
        StreamWriter streamWriter = new StreamWriter(fileName, true);
        streamWriter.WriteLine(JsonSerializer.Serialize(pizza));
        streamWriter.Close();
    }

    ///<summary>
    ///Returns all the pizzas.
    ///</summary>
    public static List<Pizza> GetAll() => pizzas;
    ///<summary>
    ///Returns a pizza from an ID. Returns null if no matching ID is found.
    ///</summary>
    public static Pizza? Get(int id) => pizzas.FirstOrDefault(p => p.Id == id);

    ///<summary>
    ///Removes a pizza from the list and from the json file.
    ///</summary>
    public static void Remove(int id)
    {
        var pizza = Get(id);
        int index = id - 1;
        if (pizza is not null)
        {
            pizzas.Remove(pizza);
            List<string> lines = new List<string>();
            foreach (string line in File.ReadAllLines(fileName))
            {
                if (JsonSerializer.Deserialize<Pizza>(line)?.Id != id)
                    lines.Add(line);
            }
            File.WriteAllLines(fileName, lines);
        }
    }

    ///<summary>
    ///Updates a Pizza that matches the parameter pizza's ID. Updates the json file.
    ///</summary>
    public static void Update(Pizza newPizza)
    {
        var index = pizzas.FindIndex(p => p.Id == newPizza.Id);
        if (index > -1)
        {
            pizzas[index] = newPizza;
            string[] lines = File.ReadAllLines(fileName);
            lines[index] = JsonSerializer.Serialize(newPizza);
            File.WriteAllLines(fileName, lines);
        }
    }
}
