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
{Application="ApplicationWeb",Extra="{ Result = 65 }"}
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

JSON parse not working
```
{Application="ApplicationWeb",Extra="{ Result = 85 }"} | line_format "{{ .Extra}}"  | json Extra="Extra"
```

JSON parse working
```
{Application="ApplicationWeb",Extra="{ }"} | line_format "{{ .Extra}}" | json New_Label="extra"
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


