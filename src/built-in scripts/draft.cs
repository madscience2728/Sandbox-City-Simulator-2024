namespace Sandbox_Simulator_2024.Scripting;


public class FireStationExampleScript
{
    public static string draft = """
### FIRE STATION EXAMPLE SCRIPT ###





### Define lists, hosts, and routers ###

# Top-level
define Road is router
define Building is host
define MotherNature is host
define ICatchOnFire is an interface that has bool IsOnFire, random CatchOnFire, action OnCatchFire, action OnExtinguishFire
define ICanBeDestroyed is an interface that has bool IsDestroyed, action BeDestroyed, action OnDestroy

# Derived
define House is Building

# Packets
define Person is packet
define FirePacket is packet
define Fire911 is packet
define FireTruck is packet
FireTruck has new random 3% RollToExtinguishFire





### Define Person ###

Person.Name = "Just a person"
Person has Stats





### Define Mother Nature ###

MotherNature.Name = "Mother Nature"
MotherNature has new random 0.1% StartFire
MotherNature.OnStep += if MotherNature.StartFire.Roll then send FirePacket to random ICatchOnFire





### Define Building ###

# Setup building
Building.Name = "Just a building"
Building has new list of people
Building has new random 1% chance to CatchFire
Building has new random 15% chance to BeDestroyed

# Setup interfaces
Building interfaces ICatchOnFire
Building interfaces ICanBeDestroyed

# Setup interface actions
Building.OnCatchFire        += print red "{Building.Name} is on fire"
Building.OnCatchFire        += Building.IsOnFire = true
Building.OnExtinguishFire   += print green "{Building.Name} is no longer on fire"
Building.OnDestroy          += if IsOnFire then print red "Fire has consumed {Building.Name}"
Building.OnDestroy          += print red "{Building.Name} is destroyed"
Building.OnDestroy          += Building.IsOnFire = false
                            
# Setup host actions
Building.OnReceivePacket    += if packet is FirePacket and not Building.IsOnFire and Building.CatchOnFire.Roll then Building.IsOnFire = true and Building.OnCatchFire
Building.OnReceivePacket    += if packet is FireTruck and FireTruck.RollToExtinguishFire then Building.IsOnFire = false and Building.OnExtinguishFire and print green "{Building.Name} has been saved by the fire department"
Building.OnStep             += if Building.IsOnFire and people > 0 then from people take random person and print red "{person.Name} has died in the {Building.Name} fire"
Building.OnStep             += if Building.IsOnFire and Building.BeDestroyed.Roll then Building.OnDestroy





### Make road networks ###

define ArterialRoads is list
define CollectorRoads is list
define ResidentialRoads is list
define Highways is list
define AllRoads is list

# Arterial Roads
MainSt = new Road
MainSt.Name = "Main Street"
ArterialRoads = create many of type Road on MainSt with 4 children
AllRoads includes ArterialRoads

# Collector Roads
CollectorRoads = create hubAndSpoke of type Road on MainRoads with 2 to 6 children
AllRoads includes CollectorRoads

# Residential Roads
ResidentialRoads = create hubAndSpoke of type Road on CollectorRoads with 2 to 6 children
AllRoads includes ResidentialRoads

# Highways
Highways = create connections of type Road on CollectorRoads with 10 to 50 children
AllRoads includes Highways





### Setup houses ###

define Houses is list
define AllPeople is list

Houses = create many of type House on ResidentialRoads with 2500 children
AllPeople = create many of type Person for Houses.people with 10000 children
""";
}