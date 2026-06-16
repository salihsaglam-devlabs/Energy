#!/bin/zsh
cd /Users/base/Codes/Energy
echo "Domain entities:  $(find Energy.Domain/Modules -path '*/Entities/*.cs' | wc -l | tr -d ' ')"
echo "Domain enums:     $(find Energy.Domain/Modules -path '*/Enums/*.cs' | wc -l | tr -d ' ')"
echo "EF configs:       $(find Energy.Infrastructure/Persistence/Configurations -name '*Configuration.cs' | wc -l | tr -d ' ')"
echo "App ifaces:       $(find Energy.Application/Modules -path '*/Services/I*Service.cs' | wc -l | tr -d ' ')"
echo "App validators:   $(find Energy.Application/Modules -path '*/Validators/*.cs' | wc -l | tr -d ' ')"
echo "App lookups:      $(find Energy.Application/Modules -path '*/Lookups/I*.cs' | wc -l | tr -d ' ')"
echo "Infra services:   $(find Energy.Infrastructure/Modules -path '*/Services/*Service.cs' | wc -l | tr -d ' ')"
echo "Infra lookups:    $(find Energy.Infrastructure/Modules -path '*/Lookups/*.cs' | wc -l | tr -d ' ')"
echo "API controllers:  $(find Energy.Api/Controllers/Modules -name '*Controller.cs' | wc -l | tr -d ' ')"
echo "Web controllers:  $(find Energy.Web/Controllers -name '*Controller.cs' | wc -l | tr -d ' ')"
echo "Web clients:      $(find Energy.Web/Clients -name '*ApiClient.cs' | wc -l | tr -d ' ')"
echo "Web view cshtml:  $(find Energy.Web/Views -name '*.cshtml' | wc -l | tr -d ' ')"
echo "JS module files:  $(find Energy.Web/wwwroot/js/modules -name '*.js' 2>/dev/null | wc -l | tr -d ' ')"
echo "Shared req/resp:  $(find Energy.Shared/Models -name '*.cs' | wc -l | tr -d ' ')"
echo "DbSets:           $(grep -c 'DbSet<' Energy.Infrastructure/Persistence/AppDbContext.cs)"
echo "Test files:       $(find Energy.Tests -name '*.cs' | wc -l | tr -d ' ')"

