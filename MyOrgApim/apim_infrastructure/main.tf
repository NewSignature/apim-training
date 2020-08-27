provider "azurerm" {
  version = "=2.8.0"
  features {}
}

# create the resource group
data "azurerm_resource_group" "rg" {
    name        = "rg-segtraining"
}

# create the APIM instance
data "azurerm_api_management" "apim" {
    name                = "apim-jfsegtraining"
    resource_group_name = data.azurerm_resource_group.rg.name
}

# define group for product access
resource "azurerm_api_management_group" "people_group" {
    name                = "people-group"
    resource_group_name = data.azurerm_resource_group.rg.name
    api_management_name = data.azurerm_api_management.apim.name
    display_name        = "PeopleGroup"
    description         = "User have access to People information APIs"
}

# create Products
resource "azurerm_api_management_product" "apim_product_std" {
    product_id              = "training-people-product-std"
    api_management_name     = data.azurerm_api_management.apim.name
    resource_group_name     = data.azurerm_resource_group.rg.name

    display_name            = "PeopleStd"
    subscription_required   = true
    approval_required       = false
    published               = true
}

resource "azurerm_api_management_product" "apim_product_unl" {
    product_id              = "training-people-product-unl"
    api_management_name     = data.azurerm_api_management.apim.name
    resource_group_name     = data.azurerm_resource_group.rg.name

    display_name            = "PeopleUnl"
    subscription_required   = true
    subscriptions_limit     = 10
    approval_required       = true
    published               = true
}

# associate products to relevant group
resource "azurerm_api_management_product_group" "people_group_people_std_product" {
    product_id          = azurerm_api_management_product.apim_product_std.product_id
    group_name          = azurerm_api_management_group.people_group.name
    api_management_name = data.azurerm_api_management.apim.name
    resource_group_name = data.azurerm_resource_group.rg.name
}

resource "azurerm_api_management_product_group" "people_group_people_unl_product" {
    product_id          = azurerm_api_management_product.apim_product_unl.product_id
    group_name          = azurerm_api_management_group.people_group.name
    api_management_name = data.azurerm_api_management.apim.name
    resource_group_name = data.azurerm_resource_group.rg.name
}

# create storage for terraform state
data "azurerm_storage_account" "storage" {
    name                        = "stgsegtrainingimages"
    resource_group_name         = data.azurerm_resource_group.rg.name
}

# create the container for the remote state
resource "azurerm_storage_container" "state_container" {
    name                    = "tfstate"
    storage_account_name    = data.azurerm_storage_account.storage.name
    container_access_type   = "private"
}