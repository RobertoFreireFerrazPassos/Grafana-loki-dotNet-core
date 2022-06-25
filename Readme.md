## Steps to Run:

### Grafana: 
```
Link: http://localhost:3000/
Default user name and password will be "admin"
Add new Data Source
Name: ApplicationWeb
Url: http://Loki:3100
click in Save & test

Go to Explore, 
In Log Browser, Run query 
{Application="ApplicationWeb"}
```

### Swagger:

Run a endpoint and see new logs in the Grafana

```
ApplicationWeb
http://localhost:4001/swagger/index.html

```

### Loki

#### Log Queries

##### Stream Selector

```
{Application="ApplicationWeb"}
```

**AND** logic:

```
Query by RequestPath AND StatusCode
{RequestPath="/ApiTest/GetResult",StatusCode="200"}
```

**=~** for regex matches:
```
Query by Application and both StatusCode
{Application="ApplicationWeb",StatusCode=~"200|400"}
```

##### Pipeline Json

It cannot filter by "Detected fields"
```
{Application="ApplicationWeb", LogId="292d74e6-3899-439d-876f-f99596d350a0"} 
```

First, it must parse using '| json', so "Detected fields" become "Log labels"
Then, it can filter.
```
{Application="ApplicationWeb"} | json | LogId="292d74e6-3899-439d-876f-f99596d350a0"
```

"| json" will produce the following mapping: 

```
{ "a.b": {c: "d"}, e: "f" } -> {a_b_c="d", e="f"}

```
Ex: {Application="ApplicationWeb"} | json

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/jsonexample2.PNG?raw=true">
</p>

It can create new labels 
```
{Application="ApplicationWeb"} | json | LogId="292d74e6-3899-439d-876f-f99596d350a0" | json new_label="Content"
```

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/jsonexample1.PNG?raw=true">
</p>


##### Optional log Pipeline (line/label filter operation)

Basic example
```
{Application ="ApplicationWeb"} | StatusCode != 200
```

It can mutate the log content
```
{Application ="ApplicationWeb"} | line_format "{{.Path}}"

{Application="ApplicationWeb",Extra="{ Result = 85 }"} | line_format "{{ .Extra}}" 
```

#### Metric Queries

Draft: try to understand this query
```
count_over_time({Application="ApplicationWeb",RequestPath="/api/Values/GetValue",SourceContext="ApplicationWeb.Controllers.ValuesController"}[5m])
```

Draft: try to understand this query
```
sum(rate({Application="ApplicationWeb",RequestPath="/api/Values/GetValue",SourceContext="ApplicationWeb.Controllers.ValuesController"} [10s]))
```

Draft: try to understand this query
```
sum(count_over_time({Application="ApplicationWeb",RequestPath="/api/Values/GetValue",SourceContext="ApplicationWeb.Controllers.ValuesController"}[10s]))
```
## References:

Project setup:

https://medium.com/c-sharp-progarmming/net-core-microservice-logging-with-grafana-and-loki-92cd2783ed88

https://henriquemauri.net/coletando-logs-com-o-serilog-no-net-6/

https://linuxblog.xyz/posts/grafana-loki/

Queries in Loki Grafana:

https://grafana.com/docs/loki/latest/logql/log_queries/

https://grafana.com/docs/loki/latest/logql/metric_queries/

https://megamorf.gitlab.io/cheat-sheets/loki/

https://sbcode.net/grafana/logql/

https://www.youtube.com/watch?v=HDpE9v1Syz8&ab_channel=SBCODE


