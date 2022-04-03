using HashidsNet;

namespace FileUpload.Shared.Services
{
    public class HashService : IHashService
    {
        private readonly Hashids _hashids;

        public HashService()
        {
            _hashids = new Hashids("my_saltkey");
        }

        public string Encode(int id)
        {
            return _hashids.Encode(id);
        }

        public int Decode(string key)
        {
            return _hashids.DecodeSingle(key);
        }
        
    }
}
