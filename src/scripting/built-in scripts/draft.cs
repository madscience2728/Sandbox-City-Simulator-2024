namespace Sandbox_Simulator_2024.Scripting;







public class FireStationExampleScript
{
    public static string draft = """
### FIRE STATION EXAMPLE SCRIPT ###

### Define lists, hosts, and routers ###

# Top-level
Road is a router.
Building is a host.
MotherNature is a host.
Houses is list.
AllPeople is list.
AllRoads is list.

# Derived
House is a Building.

# Interfaces
ICatchOnFire is an interface that has:
    bool IsOnFire,
    random CatchOnFire,
    action OnCatchFire,
    action OnExtinguishFire.
ICanBeDestroyed is an interface that has:
    bool IsDestroyed,
    action BeDestroyed,
    action OnDestroy.
ILikePie is an interface.

# Packets
Person is a packet.
Fire is a packet.
Fire911 is a packet.
FireTruck is a packet.
FireTruck has a new random 3% chance to ExtinguishFire.





### Define Person ###

Person set name to "{name generator}".





### Define Mother Nature ###

MotherNature set name to "Mother Nature".
MotherNature has a new random 1% chance to StartFire.
MotherNature implements onStep if roll StartFire then send Fire to random ICatchOnFire.





### Define Building ###

# Setup building
Building set name to "{name generator} building".
Building has:
    a new list of people,
    a new random 1% chance to CatchFire,
    a new random 15% chance to BeDestroyed.

# Setup interfaces
Building interfaces ICatchOnFire.
Building interfaces ICanBeDestroyed.

# Setup interface actions
Building implements OnExtinguishFire:
    print green "{name of Building} is no longer on fire".
Building implements OnCatchFire:
    print red "{name of Building} is on fire",
    set IsOnFire true.
Building implements OnDestroy:
    if Building IsOnFire then print red "Fire has consumed {name of Building}",
    print red "{name of Building} is destroyed",
    set IsOnFire false.
                            
# Setup host actions
Building implements onReceivePacket:
    if packet is FirePacket and not IsOnFire and roll CatchOnFire then set IsOnFire true and call OnCatchFire,
    if packet is FireTruck and roll FireTruck ExtinguishFire then:
        Building set IsOnFire false,
        Building call OnExtinguishFire,
        print green "{name of Building} has been saved by the fire department".
Building implements onStep:
    if IsOnFire then from people take random person and print red "{name of person} has died in the {name of Building} fire",
    if IsOnFire and roll BeDestroyed then call OnDestroy.





### Make road networks ###

ArterialRoads is a list.
CollectorRoads is a list.
ResidentialRoads is a list.
Highways is a list.
AllRoads is a list.

# Arterial Roads
MainSt is create a Road "Main Street".
ArterialRoads is create 4 Road on MainSt.
AllRoads includes ArterialRoads.

# Collector Roads
CollectorRoads is create hubAndSpoke of Road on MainRoads with 2 to 6 children.
AllRoads includes CollectorRoads.

# Residential Roads
ResidentialRoads is to create hubAndSpoke of type Road on CollectorRoads with 2 to 6 children.
AllRoads includes ResidentialRoads.

# Highways
Highways is to create connections of type Road on CollectorRoads with 10 to 50 children.
AllRoads includes Highways.





### Setup houses ###



Houses is create 2500 House on ResidentialRoads.
AllPeople is create 10000 Person.
# Add people to houses
# Add firestation
""";
}