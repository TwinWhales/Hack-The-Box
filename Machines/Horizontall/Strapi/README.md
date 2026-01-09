
## CVE-2019-18818

node_modules/strapi-admin/controllers/Auth.js 와
node_modules/strapi-plugin-users-permissions/controllers/Auth.js 부분에서 취약점이 존재한다.

```js
    .findOne({ resetPasswordToken: code });
```

이 부분에서 code의 형식이 지정되어있지 않기 때문에 bypass가 가능하다.


## CVE-2019-19609

execa 함수로 인해 발생한다.

```js
    async installPlugin(ctx) {
    try {
      const { plugin } = ctx.request.body;
      strapi.reload.isWatching = false;

      strapi.log.info(`Installing ${plugin}...`);
      await execa('npm', ['run', 'strapi', '--', 'install', plugin]);

      ctx.send({ ok: true });

      strapi.reload();
    } catch (err) {
      strapi.log.error(err);
      strapi.reload.isWatching = true;
      ctx.badRequest(null, [{ messages: [{ id: 'An error occurred' }] }]);
    }
  },


  async uninstallPlugin(ctx) {
    try {
      const { plugin } = ctx.params;
      strapi.reload.isWatching = false;

      strapi.log.info(`Uninstalling ${plugin}...`);
      await execa('npm', ['run', 'strapi', '--', 'uninstall', plugin, '-d']);

      ctx.send({ ok: true });

      strapi.reload();
    } catch (err) {
      strapi.log.error(err);
      strapi.reload.isWatching = true;
      ctx.badRequest(null, [{ messages: [{ id: 'An error occurred' }] }]);
    }
  },

```