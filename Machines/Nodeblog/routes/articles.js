const express = require('express')
const Article = require('./../models/article')
const libxmljs = require('libxmljs')
const fileUpload = require('express-fileupload')
const router = express.Router()

// Create New Article
router.get('/new', (req, res) => {
    res.render('articles/new', { article: new Article() })
})

// Edit Article
router.get('/edit/:id', async (req, res) => {
    const article = await Article.findById(req.params.id)
    res.render('articles/edit', { article: article })
})

// XML Import
router.post('/xml', async (req, res) => {
    try {
            var file = req.files.file
            xml = file.data.toString('ascii') 
            
            var article = new Article()

            const doc = libxmljs.parseXmlString(xml, {noent: true,noblanks:true})
            doc.root().childNodes().forEach(function(element) {
                switch (element.name()) {
                    case 'title':
                        article.title = element.text()
                        break
                    case 'description':
                        article.description = element.text()
                        break
                    case 'markdown':
                        article.markdown = element.text()
                        break
                }
            })
            
            res.render('articles/edit', { article: article })
    } catch (e) {
            res.send("Invalid XML Example: <post><title>Example Post</title><description>Example Description</description><markdown>Example Markdown</markdown></post>")
    }
})

router.get('/:slug', async (req, res) => {
    const article = await Article.findOne({ slug: req.params.slug })
    if (article == null) res.redirect('/')
    res.render('articles/show', {article: article})
})

router.post('/', async (req, res, next) => {
    req.article = new Article()
    next()
}, saveArticleAndRedirect('new'))

router.delete('/:id', async (req, res) => {
    await Article.findByIdAndDelete(req.params.id)
    res.redirect('/')    
})

router.put('/:id', async (req, res, next) => {
    req.article = await Article.findById(req.params.id)
    next()
}, saveArticleAndRedirect('edit'))

function saveArticleAndRedirect(path) {
    return async (req, res) => {
        let article = req.article
        article.title = req.body.title
        article.description = req.body.description
        article.markdown = req.body.markdown
        article.ip = req.socket.remoteAddress

        try {
            article = await article.save()
            res.redirect(`/articles/${article.slug}`)
        } catch (e) {
            console.log(e)
            res.render('articles/${path}', { article: article })
        }
    }
}
module.exports = router