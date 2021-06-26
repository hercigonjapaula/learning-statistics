library(datasets)
library(graphics)
library(ggplot2)
library(stats)
require(TeachingDemos)
args = commandArgs(trailingOnly = TRUE)
path = args[1]

## Skup podataka i x i y varijable
data <- read.csv(as.character(args[2]))
x <- as.numeric(args[3])
y <- as.numeric(args[4])

## Linearni model
fit = lm(data[,y] ~ data[,x], data = data)

## Statističko zaključivanje
s <- summary(fit)
# t-test
sink(paste(path, "ttest.txt", sep = ""))
print(s$coefficients)
sink()
# f-test
sink(paste(path, "ftest.txt", sep = ""))
print(s$fstatistic)
sink()

## Mjere kvalitete prilagodbe modela podacima
# koeficijent determinacije
r.sq <- s$r.squared
# prilagođeni koeficijent determinacije
adj.r.sq <- s$adj.r.squared
cat(r.sq, adj.r.sq, sep = " ")