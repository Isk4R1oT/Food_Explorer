namespace Food_Explorer.Models
{
    interface IAddressBilder
    {
        IAddressBilder Adres(string adres);
        IAddressBilder City(string city);
        IAddressBilder State(string state);
        IAddressBilder ZipCode(int zipcode);
        IAddressBilder Country(string country);
        Address Create();

    }
    public class Address
    {
        public string Adress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCOde { get; set; }
        public string Country { get; set; }
    }
    class AdressOrderBilder : IAddressBilder
    {
        private Address _adressOrder { get; set; }

        public AdressOrderBilder(Address adressOrder)
        {
            _adressOrder = adressOrder;
        }

        public IAddressBilder Adres(string adres)
        {
            _adressOrder.Adress = adres;
            return this;
        }

        public IAddressBilder City(string city)
        {
            _adressOrder.City = city;
            return this;
        }

        public IAddressBilder Country(string country)
        {
            _adressOrder.Country = country;
            return this;
        }

        public Address Create()
        {
            return _adressOrder;
        }

        public IAddressBilder State(string state)
        {
            _adressOrder.State = state;
            return this;
        }

        public IAddressBilder ZipCode(int zipcode)
        {
            _adressOrder.ZipCOde = zipcode;
            return this;
        }
    }
}
