library(datasets)
library(graphics)
library(ggplot2)
library(stats)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)
path = args[1]

data <- read.csv(as.character(args[2]), header = TRUE)
x <- as.numeric(args[3])
y <- as.numeric(args[4])

## Linearni model
fit = lm(data[,y] ~ data[,x], data = data)
## Koeficijenti linearnog modela
cat(fit$coefficients, " ")

## Plot podataka
png(file=paste(path, "fittedvalues_plot.png", sep = "/"))
plot(data[,x], data[,y], 
     xlab = names(data)[x], ylab = names(data)[y],
     main = "Model linearne regresije")
## Plot fitanih vrijednosti
lines(data[,x], fit$fitted.values, col='red')
dev.off()