library(datasets)
library(graphics)
library(ggplot2)
library(stats)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)
path = args[1]

## Skup podataka i x i y varijable
data <- read.csv(as.character(args[2]), stringsAsFactors = TRUE)
data.new <- read.csv(as.character(args[3]))
x <- as.numeric(args[4])
y <- as.numeric(args[5])

## Linearni model
fit = lm(cnt ~ temp, data = data)

## Vrijednosti izlaza za nove podatke
png(file=paste(path, "linreg_plots/predictedvalues_plot.png", sep = ""))
plot(data.new[,x], data.new[,y], 
     xlab = names(data)[x], ylab = names(data)[y], 
     main = "Predikcija za nove podatke")
prediction = predict.lm(fit, data.new, interval = "confidence")
lines(data.new[,x], prediction[,1], col = "red")
dev.off()