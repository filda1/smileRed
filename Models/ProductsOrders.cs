using smileRed.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smileRed.API.Models
{
    public class ProductsOrders: Product
    {
        public int OrderID { get; set; }

        public int OrderDetailsID { get; set; }

        [Display(Name = "Order Status")]
        public string OrderStatusName { get; set; }

        public int Count { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string LastName { get; set; }

        [MaxLength(20, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(300, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Address { get; set; }

        [Display(Name = "Location")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        public string Location { get; set; }

        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        public int Code { get; set; }

        [Display(Name = "Nº Door")]
        [Required(ErrorMessage = "The field {0} is requiered.")]
        public int Door { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public float Quantity { get; set; }

        [Display(Name = "Total (+Iva)")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Value { get; set; }

        [Display(Name = "Date Order")]
        [DataType(DataType.DateTime)]
        public DateTime DateOrder { get; set; }

        #region Calculated fields 
        /// <summary>
        /// Precio  + Cantidad
        /// </summary>
        public decimal PriceQuantity
        {
            get
            {
                return Price * (decimal)Quantity;
            }

        }

        /// <summary>
        ///  IVA 
        /// </summary>
        public decimal PriceVAT
        {
            get
            {
                return Price + (Price * (VAT / 100));
            }

        }

        /// <summary>
        /// Precio  + IVA * Cantidad
        /// </summary>
        public decimal PriceVATQuantity
        {
            get
            {
                return PriceVAT * (decimal)Quantity;
            }

        }
        #endregion

        [Display(Name = "Customer")]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

    }
}