
provider "azurerm" {
  version = "=2.1.0"
  features {}
}

terraform {
    backend "azurerm" {
        # filled in by azure devops runner
    }
}

# collect variables
variable "rg_name" {
  type = string
}


variable "apim_name" {
  type = string
}

variable "apim_base_name" {
  type = string
}

variable "app_service_base_url" {
  type = string
}

# resolve the products
data "azurerm_api_management_product" "apim_product_std" {
    product_id          = "${var.apim_base_name}-people-product-std"
    api_management_name = var.apim_name
    resource_group_name = var.rg_name
}

data "azurerm_api_management_product" "apim_product_unl" {
    product_id              = "${var.apim_base_name}-people-product-unl"
    api_management_name     = var.apim_name
    resource_group_name     = var.rg_name
}

# get the resource that is our target API
resource "azurerm_api_management_api" "apim_people_api" {
  name                = "people-api"
  resource_group_name = var.rg_name
  api_management_name = var.apim_name

  revision            = 1
  display_name        = "People API"
  path                = "people"
  protocols           = [ "https" ]
  service_url         = var.app_service_base_url

  #import {
  #  content_format    = "openapi-link"
  #  content_value     = "${var.app_service_base_url}/swagger/v1/swagger.json"
  #}
}
