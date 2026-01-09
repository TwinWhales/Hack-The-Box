const express = require('express')
const User = require('./../models/user')
const Article = require('./../models/article')
const serialize = require('node-serialize')
const crypto = require('crypto')
const cookie_secret = "UHC-SecretCookie"
const router = express.Router()

// Create New Article
router.get('/', async (req, res) => {
    res.render('user/login', { user: new User() })
})

// Lazy way to create the first user
router.get('/create', async (req, res) => {
    let user = new User()
    user.username = 'admin'
    user.password = 'IppsecSaysPleaseSubscribe'
    u = await user.save()
    res.render('user/login')})

router.post('/', async (req, res) => {    
    let user = await User.findOne({username: (req.body.user)})
    if (!user) {
        msg = "Invalid Username"
        res.render('user/login', { msg: msg } )
    } else {
        let auth = await User.findOne({username: (req.body.user), password: (req.body.password) })
        if (auth) {
            logged_in = {
                user: req.body.user,
                sign: crypto.createHash('md5').update(cookie_secret + req.body.user).digest('hex')
            }
            var logged_in = serialize.serialize(logged_in)
            res.cookie("auth", logged_in, { maxAge: 900000, httpOnly: true })
            const articles = await Article.find().sort({
                createdAt: 'desc'
            })
            res.render('articles/index', { articles: articles, ip: req.socket.remoteAddress, authenticated: true })
        } else {
            msg = "Invalid Password"
            res.render('user/login', { msg: msg })
        }
    }
})

module.exports = router