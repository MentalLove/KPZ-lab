using System;
using System.Collections.Generic;

namespace DesignPatterns.Mediator
{
    interface IMediator
    {
        bool RequestLanding(Aircraft aircraft);
        bool RequestTakeOff(Aircraft aircraft);
    }

    class Aircraft
    {
        public string Name;
        public bool IsTakingOff { get; set; }
        private IMediator _mediator;

        public Aircraft(string name, IMediator mediator)
        {
            this.Name = name;
            this._mediator = mediator;
        }

        public void Land()
        {
            Console.WriteLine($"Aircraft {this.Name} requests landing.");
            if (_mediator.RequestLanding(this))
            {
                Console.WriteLine($"Aircraft {this.Name} has landed.");
            }
            else
            {
                Console.WriteLine($"Aircraft {this.Name} could not land. No free runway.");
            }
        }

        public void TakeOff()
        {
            Console.WriteLine($"Aircraft {this.Name} requests takeoff.");
            if (_mediator.RequestTakeOff(this))
            {
                Console.WriteLine($"Aircraft {this.Name} has taken off.");
            }
            else
            {
                Console.WriteLine($"Aircraft {this.Name} could not take off.");
            }
        }
    }

    class Runway
    {
        public readonly Guid Id = Guid.NewGuid();
        public Aircraft? IsBusyWithAircraft;

        public bool IsFree()
        {
            return IsBusyWithAircraft == null;
        }

        public void Occupy(Aircraft aircraft)
        {
            IsBusyWithAircraft = aircraft;
            HighLightRed();
        }

        public void Release()
        {
            IsBusyWithAircraft = null;
            HighLightGreen();
        }

        public void HighLightRed()
        {
            Console.WriteLine($"Runway {this.Id} is busy!");
        }

        public void HighLightGreen()
        {
            Console.WriteLine($"Runway {this.Id} is free!");
        }
    }

    class CommandCentre : IMediator
    {
        private List<Runway> _runways = new List<Runway>();
        private Dictionary<Aircraft, Runway> _aircraftRunwayMap = new Dictionary<Aircraft, Runway>();

        public CommandCentre(Runway[] runways)
        {
            this._runways.AddRange(runways);
        }

        public bool RequestLanding(Aircraft aircraft)
        {
            foreach (var runway in _runways)
            {
                if (runway.IsFree())
                {
                    runway.Occupy(aircraft);
                    _aircraftRunwayMap[aircraft] = runway;
                    return true;
                }
            }
            return false;
        }

        public bool RequestTakeOff(Aircraft aircraft)
        {
            if (_aircraftRunwayMap.ContainsKey(aircraft))
            {
                var runway = _aircraftRunwayMap[aircraft];
                runway.Release();
                _aircraftRunwayMap.Remove(aircraft);
                return true;
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Створюємо злітно-посадкові смуги
            Runway runway1 = new Runway();
            Runway runway2 = new Runway();

            // Створюємо центр управління
            CommandCentre commandCentre = new CommandCentre(new Runway[] { runway1, runway2 });

            // Створюємо літаки
            Aircraft aircraft1 = new Aircraft("Boeing 737", commandCentre);
            Aircraft aircraft2 = new Aircraft("Airbus A320", commandCentre);
            Aircraft aircraft3 = new Aircraft("Cessna 172", commandCentre);

            // Демонстрація роботи системи
            aircraft1.Land();
            aircraft2.Land();
            aircraft3.Land(); // має не знайти вільної смуги

            aircraft1.TakeOff();
            aircraft3.Land(); // тепер має знайти вільну смугу

            Console.WriteLine("Simulation finished.");
        }
    }
}
