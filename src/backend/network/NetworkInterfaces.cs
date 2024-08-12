namespace Network.Core;

public interface IUDP { }

public interface IEntity { }

public interface IName
{
    string Name { get; set; }
}

public interface IGenerateNetworks
{
    public Task GenerateNetwork();
}