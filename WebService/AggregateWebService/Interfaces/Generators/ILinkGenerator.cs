using System.Collections.Generic;
using AggregateWebService.Models;

namespace AggregateWebService.Interfaces.Generators
{
    public interface ILinkGenerator<T>
    {
        IEnumerable<Link> GenerateCollectionLinks(T aggregate);
        IEnumerable<Link> GenerateItemLinks(T aggregate);
    }
}
