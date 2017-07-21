using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CompetitiveProgramming
{
    public class Reflection
    {
        readonly List<Person> _listOfPersons = new List<Person>
        {
            new A(),
            new B(),
            new C()
        };

        public void PrintPersonAge()
        {
            dynamic d = _listOfPersons;
            foreach (var person in d)
            {
                try
                {
                    int value = person.Age;
                    Debug.WriteLine(value);
                }
                catch (Exception e)
                {
                  
                }
            }
        }

        public string GetAge(Person p, string propertyName)
        {
            Type t = p.GetType();
            var property = t.GetProperty(propertyName);
            if (property != null)
            {
                var value =  (int)property.GetValue(p);
                return value.ToString();
            }

            return string.Empty;
        }
    }

    public class Person
    {

    }

    public class A : Person
    {
        public int Age { get; set; } = 100;
        public override string ToString()
        {
            return $"Age of A is {Age}";
        }
    }

    public class B : Person
    {
        public int Age { get; set; } = 20;

        public override string ToString()
        {
            return $"Age of B is {Age}";
        }
    }

    public class C : Person
    {
        public int Age { get; set; } = 200;
    }
}
