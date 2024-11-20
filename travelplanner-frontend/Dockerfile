FROM node:22.11
LABEL authors="auste"
RUN mkdir -p /usr/src/travelplanner-frontend
WORKDIR /usr/src/travelplanner-frontend
COPY . .
RUN npm i
RUN npm cache clean --force
RUN npm run build
EXPOSE 3000
CMD ["npm", "run", "start"]