using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace Laba14
{
    [Serializable]
    public abstract class Vehicle
    {
        [NonSerialized]
        public int Wheels;
        public abstract string Driver();

    }

    [Serializable]
    public class Car : Vehicle
    {
        public int Power;
        public Car()
        {
            Console.WriteLine("Введите мощность двигателя");
            Power = Convert.ToInt32(Console.ReadLine());
            Wheels = 4;
        }

        public override string Driver()
        {
            return "Водитель управляет автомобилем";
        }
        public bool IsThereAnEngine()
        {
            return true;
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public string Speciality { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {

            //1 задание
            Car car = new Car();

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("Car.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, car);
            }

            using (FileStream fs = new FileStream("Car.dat", FileMode.OpenOrCreate))
            {
                Car newCar = (Car)formatter.Deserialize(fs);
                Console.WriteLine($"Мощность двигателя нового автомобиля: {newCar.Power} \nКоличество колес {newCar.Wheels}");
            }

            SoapFormatter soapFormatter = new SoapFormatter();

            using(Stream fstream = new FileStream("soapCar.dat", FileMode.OpenOrCreate))
            {
                soapFormatter.Serialize(fstream, car);
            }

            using(Stream fstream = new FileStream("soapCar.dat", FileMode.OpenOrCreate))
            {
                Car soapCar = (Car)soapFormatter.Deserialize(fstream);
                Console.WriteLine("Мощность soap автомобиля: {0}\nКоличество колес {1}", soapCar.Power, soapCar.Wheels);
            }

            XmlSerializer xSer = new XmlSerializer(typeof(Car));
            using (FileStream fs = new FileStream("Car.xml", FileMode.OpenOrCreate))
            {
                xSer.Serialize(fs, car);
            }

            using (FileStream fs = new FileStream("Car.xml", FileMode.OpenOrCreate))
            {
                Car xmlCar = (Car)xSer.Deserialize(fs);
                Console.WriteLine($"Мощность двигателя xml автомобиля: {xmlCar.Power}\nКоличество колес {xmlCar.Wheels}");
            }

            string json = JsonSerializer.Serialize(car);
            Car jsonCar = JsonSerializer.Deserialize<Car>(json);
            Console.WriteLine($"Мощность двигателя json автомобиля: {jsonCar.Power}\nКоличество колес {jsonCar.Wheels}");
            //2 задание


            List<Car> cars = new List<Car>();
            cars.Add(car);
            cars.Add(car);
            cars.Add(car);
            XmlSerializer xser = new XmlSerializer(typeof(List<Car>));
            using (FileStream fileStream = new FileStream("Cars.xml", FileMode.OpenOrCreate))
            {
                xser.Serialize(fileStream, cars);
            }
            using (FileStream fileStream = new FileStream("Cars.xml", FileMode.OpenOrCreate))
            {
                List<Car> ball15 = (List<Car>)xser.Deserialize(fileStream);
                foreach (Car c in cars)
                {
                    Console.WriteLine($"Мощность двигателя xml автомобиля: {c.Power}\nКоличество колес {c.Wheels}");
                }
            }

            //Задание 3

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("Students.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            XmlNodeList childnodes = xRoot.SelectNodes("student");
            foreach (XmlNode n in childnodes)
                Console.WriteLine(n.SelectSingleNode("@name").Value);
            childnodes = xRoot.SelectNodes("student[speciality='ДЭиВИ']");
            foreach (XmlNode n in childnodes)
                Console.WriteLine(n.InnerText);

            //Задание 4
            XDocument xdoc = XDocument.Load("Students.xml");
            foreach (XElement stud in xdoc.Element("students").Elements("student"))
            {
                XAttribute nameAttribute = stud.Attribute("name");
                XElement specialityElement = stud.Element("speciality");
                XElement ageElement = stud.Element("age");

                if (nameAttribute != null && specialityElement != null && ageElement != null)
                {
                    Console.WriteLine($"Student: {nameAttribute.Value}");
                    Console.WriteLine($"Speciality: {specialityElement.Value}");
                    Console.WriteLine($"Age: {ageElement.Value}");
                }
                Console.WriteLine();
            }
            var items = from xe in xdoc.Element("students").Elements("student")
                        where xe.Element("speciality").Value == "ИСиТ"
                        select new Student
                        {
                            Name = xe.Attribute("name").Value,
                            Speciality = xe.Element("speciality").Value
                        };

            foreach (var item in items)
                Console.WriteLine($"{item.Name} - {item.Speciality}");
        }
    }
}
