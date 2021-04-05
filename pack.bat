for %%p in (Itamaram.SpecFlow.Plugin.Base Itamaram.Csv.SpecFlowPlugin Itamaram.Excel.SpecFlowPlugin) do (
	nuget pack %%p\%%p.csproj -Build -Prop Configuration=Release -Version %1 -Verbosity detailed
)