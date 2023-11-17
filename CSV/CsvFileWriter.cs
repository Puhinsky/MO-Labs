using System.Text;

namespace CSV
{
    public class CsvFileWriter
    {
        private readonly string _path;
        private readonly string _fileName;

        private string _result = "";

        public CsvFileWriter(string path, string fileName)
        {
            _path = path;
            _fileName = fileName;
        }

        public void AddData<T>(IEnumerable<T> data)
        {
            _result += string.Join(";", data) + "\n";
        }

        public void Save()
        {
            File.WriteAllText($"{_path}\\{_fileName}.csv", _result, Encoding.UTF32);
        }
    }
}