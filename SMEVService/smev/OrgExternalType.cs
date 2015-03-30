using System;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace SMEVService.smev
{
    /// <summary>
    /// Сведения об информационной системе
    /// </summary>
    [MessageContract]
    public class OrgExternalType
    {
        private string _code;

        public OrgExternalType()
        {
        }

        public OrgExternalType(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public string Code
        {
            get { return _code; }
            set
            {
                if (!Regex.IsMatch(value, @"[A-Z0-9]{4}\d{5}"))
                    throw new FormatException(@"Не верный формат должно быть: [A-Z0-9]{4}\d{5}");
                _code = value;
            }
        }

        public string Name { get; set; }
    }
}