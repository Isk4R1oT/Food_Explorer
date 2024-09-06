using Food_Explorer.Data_Access_Layer.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace Food_Explorer.Data_Access_Layer.Builders
{
    public interface IProductBuilder
    {
        IProductBuilder Name(string name);
        IProductBuilder Description(string descr);
        IProductBuilder Ingredients(string ingredients);
        IProductBuilder Price(int price);
        IProductBuilder Type(int type);
        IProductBuilder Quantity(int quantity);
        IProductBuilder Image(string image);
        Product Create();
    }

    public static class ProductFactory
    {
        public static Product CreateProduct(ProductType productType)
        {
            switch (productType)
            {
                case ProductType.Entree: return new Entree();
                case ProductType.Drink: return new Drink();
                case ProductType.Dessert: return new Dessert();
                default: throw new ArgumentException($"Invalid ProductType: {productType}");
            }
        }
    }
    public class ProductBuilder : IProductBuilder
    {
        private readonly Product _product;
        public ProductBuilder(ProductType productType)
        {
            _product = ProductFactory.CreateProduct(productType);
        }
        public Product Create()
        {
            return _product;
        }

        public IProductBuilder Description(string descr)
        {
            _product.Description = descr;
            return this;
        }


        public IProductBuilder Ingredients(string ingredients)
        {
            _product.Ingredients = ingredients;
            return this;
        }


        public IProductBuilder Name(string name)
        {
            _product.Name = name;
            return this;
        }


        public IProductBuilder Price(int price)
        {
            _product.Price = price;
            return this;
        }


        public IProductBuilder Quantity(int quantity)
        {
            _product.Quantity = quantity;
            return this;
        }


        public IProductBuilder Type(int type)
        {
            _product.ProductType = (ProductType)type;
            return this;
        }
        public IProductBuilder Image(string image)
        {
            _product.Image = image;
            return this;
        }
    }
}
