dev: ## Run Development build
	@ASPNETCORE_ENVIRONMENT=development dotnet watch run

prod: ## Run production build
	@ASPNETCORE_ENVIRONMENT=production dotnet run

migrate: ## Run migrations
	@dotnet ef database update 

help: ## Show this help.
	@fgrep -h "##" $(MAKEFILE_LIST) | fgrep -v fgrep | sed -e 's/\\$$//' | sed -e 's/##//'

.PHONY: help
.DEFAULT_GOAL := help
