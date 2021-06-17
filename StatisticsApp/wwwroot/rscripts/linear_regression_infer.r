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

## q-q plot reziduala s linijom normalne distribucije
png(file=paste(path, "linreg_plots/qqplot.png", sep = ""))
qqnorm(rstandard(fit))
qqline(rstandard(fit))

## Kolmogorov - Smirnovljev test
ks.test(rstandard(fit),'pnorm')

## Statističko zaključivanje
s <- summary(fit)
# t-test
s$coefficients
# f-test
s$fstatistic

## Mjere kvalitete prilagodbe modela podacima
# koeficijent determinacije
s$r.squared
# prilagođeni koeficijent determinacije
s$adj.r.squared