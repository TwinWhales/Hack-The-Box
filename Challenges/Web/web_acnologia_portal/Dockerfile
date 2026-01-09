FROM python@sha256:81e1ea427b1c82b4e4d5c5c2761a085b8ecba36ca3d352fb739d03d875af4dfd

# Install packages
RUN apk add --update --no-cache supervisor chromium chromium-chromedriver gcc musl-dev libffi-dev

# Upgrade pip
RUN python -m pip install --upgrade pip

# Install dependencies
RUN pip install selenium==4.2.0 Flask Flask-Session Flask-SQLAlchemy SQLAlchemy-serializer Flask-Login

# Copy flag
COPY flag.txt /root/flag

# Add readflag binary
COPY config/readflag.c /
RUN gcc -o /readflag /readflag.c && chmod 4755 /readflag && rm /readflag.c

# add user
RUN adduser -D -u 1000 -g 1000 -s /bin/sh www

# Setup app
RUN mkdir -p /app

# Switch working environment
WORKDIR /app

# Add application
COPY challenge .
RUN chown -R www: /app/flask_session /app/application/static

# Setup supervisor
COPY config/supervisord.conf /etc/supervisord.conf

# Expose port the server is reachable on
EXPOSE 1337

# Disable pycache
ENV PYTHONDONTWRITEBYTECODE=1

# Run supervisord
CMD ["/usr/bin/supervisord", "-c", "/etc/supervisord.conf"]