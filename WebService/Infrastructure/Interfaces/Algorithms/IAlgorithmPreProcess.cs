using Infrastructure.Algorithms;

namespace Infrastructure.Interfaces.Algorithms
{
    public interface IAlgorithmPreProcess
    {
        /// <summary>
        /// Applies the selected preprocessing to the specified string
        /// </summary>
        /// <param name="input">String to be processed</param>
        /// <param name="preProcessType">Type of PreProcess</param>
        /// <returns>Processed Input string</returns>
        string ApplyPreProcess(string input, PreProcessTypesEnum preProcessType);
    }
}
