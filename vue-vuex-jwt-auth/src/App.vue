<template>
  <div id="app">
    <nav class="navbar navbar-expand navbar-dark bg-dark">
      <router-link to="/" class="nav-link navbar-brand">
        {{ this.siteTitle }}
      </router-link>
      <div class="navbar-nav mr-auto">
        <li class="nav-item mr-3">
          <router-link to="/home" class="nav-link">
            <font-awesome-icon icon="home" />&nbsp;トップ
          </router-link>
        </li>
        <li class="nav-item">
          <router-link v-if="currentUser" to="/profile" class="nav-link">
            <font-awesome-icon icon="file" />&nbsp;プロフィール
          </router-link>
        </li>
      </div>

      <div v-if="!currentUser" class="navbar-nav ml-auto">
        <li class="nav-item mr-3">
          <router-link to="/register" class="nav-link">
            <font-awesome-icon icon="user-plus" />&nbsp;登録
          </router-link>
        </li>
        <li class="nav-item">
          <router-link to="/login" class="nav-link">
            <font-awesome-icon icon="sign-in-alt" />ログイン
          </router-link>
        </li>
      </div>

      <div v-if="currentUser" class="navbar-nav ml-auto">
        <li class="nav-item mr-3">
          <router-link to="/profile" class="nav-link">
            <font-awesome-icon icon="user" />
            &nbsp;{{ currentUser.username }}
          </router-link>
        </li>
        <li class="nav-item">
          <a class="nav-link" href @click.prevent="logOut">
            <font-awesome-icon icon="sign-out-alt" />&nbsp;ログアウト
          </a>
        </li>
      </div>
    </nav>

    <div class="container">
      <router-view />
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      siteTitle: 'JWT 認証サンプル',
    };
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
  },
  methods: {
    logOut() {
      this.$store.dispatch('auth/logout');
      this.$router.push('/login');
    },
    createPageTitle: function (to) {
      // タイトルを設定
      if (to.meta.title) {
        const setTitle = `${to.meta.title} | ${this.siteTitle}`;
        document.title = setTitle;
      } else {
        document.title = this.siteTitle;
      }

      // メタタグdescription設定
      if (to.meta.desc) {
        var setDesc = to.meta.desc + ' | SourceAcademy';
        document
          .querySelector("meta[name='description']")
          .setAttribute('content', setDesc);
      } else {
        document
          .querySelector("meta[name='description']")
          .setAttribute('content', 'SourceAcademy');
      }
    },
  },
  mounted: function () {
    var to = this.$route;
    this.createPageTitle(to);
  },
  watch: {
    $route(to, from) {
      this.createPageTitle(to);
    },
  },
};
</script>
