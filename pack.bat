for %%p in (Itamaram.SpecFlow.Plugin.Base Itamaram.Csv.SpecFlowPlugin Itamaram.Excel.SpecFlowPlugin) do (
	dotnet pack %%p\%%p.csproj -c Release -p:Version=%1 -o .
)