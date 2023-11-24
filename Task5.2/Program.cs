using System;
using System.Collections.Generic;
using System.Linq;

// Observer Interface
public interface IObserver
{
    void Update(float temperature, float humidity, float pressure);
}

// Subject Interface
public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}

// Concrete Observer
public class ConcreteObserver : IObserver
{
    public void Update(float temperature, float humidity, float pressure)
    {
        Console.WriteLine($"Current conditions: {temperature} degrees and {humidity}% humidity");
    }
}

// Concrete Subject
public class WeatherData : ISubject
{
    private List<IObserver> observers;
    private List<float> temperatures;
    private float humidity;
    private float pressure;

    public WeatherData()
    {
        observers = new List<IObserver>();
        temperatures = new List<float>();
    }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(temperatures.Last(), humidity, pressure);
        }
    }

    public void MeasurementsChanged(float temperature, float humidity, float pressure)
    {
        // Simulate data change
        temperatures.Add(temperature);
        this.humidity = humidity;
        this.pressure = pressure;

        NotifyObservers();
    }
}

// Concrete Display
public class CurrentConditionsDisplay : IObserver
{
    private List<float> temperatures;
    private float humidity; 
    private ISubject weatherData;

    public CurrentConditionsDisplay(ISubject weatherData)
    {
        this.weatherData = weatherData;
        this.weatherData.RegisterObserver(this);
        temperatures = new List<float>();
    }

    public void Update(float temperature, float humidity, float pressure)
    {
        this.humidity = humidity; 
        temperatures.Add(temperature);
        Display();
    }

    public void Display()
    {
        float avgTemp = temperatures.Average();
        float maxTemp = temperatures.Max();
        float minTemp = temperatures.Min();

        Console.WriteLine($"Current conditions: {temperatures.Last()} degrees and {humidity}% humidity");
        Console.WriteLine($"Avg/Max/Min temperature: {avgTemp}/{maxTemp}/{minTemp}");
        Console.WriteLine("Forecast: " + GetForecast());
        Console.WriteLine();
    }

    private string GetForecast()
    {
        if (humidity > 80)
        {
            return "Watch out for cooler, rainy weather";
        }
        else if (temperatures.Last() > 75)
        {
            return "Improving weather on the way!";
        }
        else
        {
            return "More of the same";
        }
    }
}

class Program
{
    static void Main()
    {
        WeatherData weatherData = new WeatherData();
        CurrentConditionsDisplay currentConditionsDisplay = new CurrentConditionsDisplay(weatherData);

        // Trigger the display by calling MeasurementsChanged with different values
        weatherData.MeasurementsChanged(80.0f, 65.0f, 1010.0f);
        weatherData.MeasurementsChanged(82.0f, 70.0f, 1012.0f);
        weatherData.MeasurementsChanged(78.0f, 90.0f, 1005.0f);

        Console.ReadKey();
    }
}
