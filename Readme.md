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

```
http://localhost:8085/swagger/index.html

Run a endpoint and see new logs in the Grafana
```

### Loki

#### Log Queries

**AND** logic:

```
Query by RequestPath AND StatusCode
{RequestPath="/ApiTest/Simple/GetResult",StatusCode="200"}
```

**=~** for regex matches:
```
Query by Application and both StatusCode
{Application="ApplicationWeb",StatusCode=~"200|400"}
```

#### Metric Queries


Draft: try to understand this query
```
count_over_time({Application="ApplicationWeb",RequestPath="/api/Values/GetValue",SourceContext="ApplicationWeb.Controllers.ValuesController"}[5m])
```

## References:

Project setup:

https://medium.com/c-sharp-progarmming/net-core-microservice-logging-with-grafana-and-loki-92cd2783ed88

https://henriquemauri.net/coletando-logs-com-o-serilog-no-net-6/

Queries in Loki Grafana:

https://grafana.com/docs/loki/latest/logql/log_queries/

https://grafana.com/docs/loki/latest/logql/metric_queries/

https://megamorf.gitlab.io/cheat-sheets/loki/


