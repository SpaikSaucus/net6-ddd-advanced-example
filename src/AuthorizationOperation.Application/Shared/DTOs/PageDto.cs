using System.Collections.Generic;

namespace AuthorizationOperation.Application.Shared.DTOs
{
    public class PageDto<T> where T : class
    {
        public int Total { get; set; }

        public uint Offset { get; set; }

        public ushort Limit { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
