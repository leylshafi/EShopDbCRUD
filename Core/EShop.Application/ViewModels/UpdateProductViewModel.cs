﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.ViewModels
{
	public class UpdateProductViewModel
	{
		public Guid Id { get; set; }	
		public string Name { get; set; }
		public string Desc { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
	}
}
