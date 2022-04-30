namespace Britt2021.D.InterfacesFactories.Dependencies.MathNet.Numerics.Distributions
{
    using global::MathNet.Numerics.Distributions;

    public interface IDiscreteUniformFactory
    {
        IDiscreteDistribution Create(
            int lower,
            int upper);
    }
}