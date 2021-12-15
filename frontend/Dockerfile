# Use official node image as the base image
FROM node:latest as build

# Set the working directory
WORKDIR /usr/local/app

# Add the source code to app
COPY ./ /usr/local/app/

# Install all the dependencies
RUN npm install

# Generate the build of the application
RUN npm run build


# Use official nginx image as the base image
FROM nginx:latest

# Copy the build output to replace the default nginx contents.
COPY --from=build /usr/local/app/dist/frontend /usr/share/nginx/html

# Expose port 80
EXPOSE 80

CMD ["/bin/bash", "-c", \
"echo API_URL=[$API_URL], && \
sed -i s#APP_API_URL#$API_URL#g /usr/share/nginx/html/main.*.js && \
nginx -g 'daemon off;'"]
