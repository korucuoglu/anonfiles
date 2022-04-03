using HashidsNet;

namespace FileUpload.Shared.Services
{
    public class HashService : IHashService
    {
        private readonly Hashids _hashids;

        public HashService()
        {
            _hashids = new Hashids("my_saltkey", 8);
        }

        public string Encode(int id)
        {
            return _hashids.Encode(id);
        }

        public int Decode(string key)
        {
            if (_hashids.TryDecodeSingle(key, out int number))
            {
                return number;
            }

            return 0;
        }
    }

}


